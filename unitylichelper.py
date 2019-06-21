#!/usr/bin/env python3

import os
import sys
import threading
import signal
import click
import time
from subprocess import Popen, PIPE
import xml.etree.ElementTree as et

logfilename = './unitylicensehelper.log'

error = 0
logprefix = 'unitylicensehelper'
unityapp = '/Applications/Unity/Unity.app/Contents/MacOS/Unity'
licensefilename = '/Library/Application support/Unity/Unity_lic.ulf'
unityreleasecall = [unityapp, '-quit', '-batchmode', '-returnlicense']
unityacquirecall = [unityapp, '-quit', '-batchmode']

RETRIES_NUM = 3
TIMEOUT = 180
TIMEDOUT_MESSAGE = 'Call to unity was timed out. You may want to increase timeout via TIMEOUT variable.'

""" unity proccess instance """
unityproc = None




"""
getserial(licensefilename):
- parses .XML unity license file, returns unity masked serial
"""
def getserial(licensefilename):

	serial = ''

	try:

		#with open(licensefilename, 'r') as f:
		#	xmlbuffer = f.read()
		xmlroot = et.parse(licensefilename).getroot()
		sectionlst = xmlroot.findall('License/SerialMasked')

		for section in sectionlst:

			serial = section.get('Value')

	except Exception as e:
		log('Warning: could not read '+licensefilename+' reason - '+str(e))

	return serial




"""
log(message)
- logs message to console and to logfile
"""
def log(message):

	try:
		outbuffer = time.strftime('%H:%M %m.%d.%Y') + ' ' + message
	except Exception as e:
		outbuffer = message

	""" logs to console without datetime, as it goes into teamcity log which has datetime """
	print(logprefix + ' ' + message)

	with open(logfilename, 'a+') as f:
		f.write(str(outbuffer)+'\n')



"""
acquire()
"""
def acquire(serial, username, password):

	global unityproc, unityacquirecall, error

	unityacquirecall = unityacquirecall + ['-serial', serial, '-username', "'"+username+"'", '-password', "'"+password+"'" ]
	buffer = '#!/usr/bin/env bash\n'
	for param in unityacquirecall:

		buffer += param + ' '

	bashwrapscriptname = './licensehelper.sh'
	try:

		with open(bashwrapscriptname, 'w') as f:
			f.write(buffer)

	except Exception as e:
		log(str(e))
		error = 2
		return

	os.chmod(bashwrapscriptname, 0o755)
	try:

		unityproc = Popen(bashwrapscriptname, env=os.environ, stdout=PIPE, stderr=PIPE, shell=False)
		log('Unity pid '+str(unityproc.pid)+' was spawned')
		unityproc.wait()

		#(stdout, stderr) = unityproc.communicate()
		#print(stdout + ' ' + stderr)
		error = unityproc.returncode

	except Exception as e:
		error = 2
		log(str(e))

	try:
		os.remove(bashwrapscriptname)
	except:
		pass



"""
release()
"""
def release():

	global unityproc, unityreleasecall, error

	try:

		unityproc = Popen(unityreleasecall, env=os.environ, stdout=PIPE, stderr=PIPE, shell=False)
		log('Unity pid '+str(unityproc.pid)+' was spawned')

		(stdout, stderr) = unityproc.communicate()
		#print(stdout + ' ' + stderr)
		error = unityproc.returncode

	except Exception as e:
		error = 2
		log(str(e))




"""
main()
"""
@click.command()
@click.option('--action', help='action name [acquire/release]', required=True)
@click.option('--serial', help='serial of Unity license', required=False)
@click.option('--username', help='Unity user with free unity seat', required=False)
@click.option('--password', help='Unity user password', required=False)
def main(action, serial, username, password):

	global unityproc
	global error
	is_licensefile = None
	xmlserial = 'could not parse serial'

	""" checks unity license file presence """
	is_licensefile = os.path.exists(licensefilename)
	if(is_licensefile):
		xmlserial = getserial(licensefilename)

	if(action == 'acquire' and is_licensefile == False):

		"""
		has acquire command and Unity license file is absent
		lets go for a Unity seat
		"""
		if(username == None):

			log('username parameter is mandatory if action equals acquire')
			return 1

		if(password == None):

			log('password parameter is mandatory if action equals acquire')
			return 1

		if(serial == None):

			log('serial parameter is mandatory if action equals acquire')
			return 1

		retry = 1
		while retry <= RETRIES_NUM:

			log('Acquiring Unity seat from '+username+' retry number ['+str(retry)+']...')
			t = threading.Thread(target=acquire, args=(serial, username, password))
			t.start()
			time.sleep(1)
			retry += 1

			exectime = 0
			while t.is_alive():

				exectime += 1
				if(exectime >= TIMEOUT):
					break

				time.sleep(1)


			if(t.is_alive() == True):
				log('Execution timeout '+str(TIMEOUT)+'s has been reached, aborting...')
				os.kill(unityproc.pid, signal.SIGTERM)
				time.sleep(1)

			""" checks unity license file presence """
			is_licensefile = os.path.exists(licensefilename)
			if(error == 0 and is_licensefile == True):

				log('Unity seat has been acquired, serial number - '+getserial(licensefilename))
				break

			elif(error == 0 and is_licensefile == False):

				log('Unity seat was not acquired, there is no license file '+licensefilename)
				error = 1

			elif(error != 0 and is_licensefile == True):

				log('Unity seat has been acquired, yet Unity returned code was '+str(error))
				error = 0
				break

			elif(error != 0 and is_licensefile == False):

				log('Unity seat was not acuired, Unity returned non zero exit code - '+str(error))


	elif(action == 'acquire' and is_licensefile == True):

		"""
		has acquire command and Unity license file is present
		we have license file, there is nothing to do
		"""
		log('Warning: could not activate unity seat while unity license file is present - '+licensefilename+' serial - '+xmlserial)

	elif(action == 'release' and is_licensefile == True):

		"""
		we have release command and Unity license file is present
		lets go and release it
		"""
		log('Unity license file was found - '+licensefilename+' serial - '+xmlserial)
		retry = 1
		while retry <= RETRIES_NUM:

			log('Releasing Unity license, retry number ['+str(retry)+']...')
			t = threading.Thread(target=release)
			t.start()
			time.sleep(1)
			retry += 1

			exectime = 0
			while t.is_alive():

				exectime += 1
				if(exectime >= TIMEOUT):
					break

				time.sleep(1)


			if(t.is_alive() == True):
				log('Execution timeout '+TIMEOUT+'s has been reached, aborting...')
				os.kill(unityproc.pid, signal.SIGTERM)
				time.sleep(1)

			""" checks unity license file presence """
			is_licensefile = os.path.exists(licensefilename)
			if(error == 0 and is_licensefile == False):

				log('Unity seat was released.')
				break

			elif(error == 0 and is_licensefile == True):

				log('Unity seat was not released, there is license file - '+licensefilename)
				error = 1

			elif(error != 0 and is_licensefile == False):

				log('Unity seat has been released, yet Unity returned non zero code '+str(error))
				error = 0
				break

			elif(error != 0 and is_licensefile == True):

				log('Unity seat was not released, there is license file - '+licensefilename+', Unity returned non zero exit code - '+str(error))


	elif(action == 'release' and is_licensefile == False):

		log('Unity license file was not found - '+licensefilename)

	elif(action == 'check' and is_licensefile == True):

		log('Unity license file was found - '+licensefilename)
		log('Unity license file serial is '+xmlserial)

	elif(action == 'check' and is_licensefile == False):

		log('Unity license file was not found - '+licensefilename)
		error = 1

	else:

		log('action '+action+' is not supported')


	exit(error)


if(__name__ == '__main__'):
	main()
