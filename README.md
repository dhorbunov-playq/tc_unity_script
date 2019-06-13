TC Unity Script
====================================

Principe of work
---------------------

### Step 1: Installation ###

`TC Unity Script` can be added already added to project to use it for convenient build running. So `TC` should check if it's exists and copy to the project from this repository, if not. In addition it should check is `Newtonsoft.Json.dll` existed in the project becouse it possible project use `Newtonsoft.Json` libruary, but don't use `TC Unity Script`.


### Step 2: Run Unity ###

`TC` call `Unity` with command line arguments:
```
"%Unity%" -projectPath "%ProjectPath%" -logfile %LogFile% -executeMethod TCUnityBuild.Build -buildSteps %BuildSteps% &
```
Where:
* `%Unity%` - path to unity, for example `/Applications/Unity/Unity 5.6.3.app/Contents/MacOS/Unity`;
* `%ProjectPath%` - path to Unity project, for example "~/Documents/Repositories/CharmKing";
* `%LogFile%` - file to write logs, for axample `~/Documents/Repositories/CharmKing/build/BuildLog.txt`;
* `-executeMethod TCUnityBuild.Build` - say Unity what method it should call. `TCUnityBuild.Build` is entry point for steps runner. You shouldn't change this one;
* `%BuildSteps%` - data for `TC Unity Script` in `json` format. It containce all needed steps, versions, keys and another data. Formating (spaces, line brakes) doesn't matter;
* `&` - should be to run `Unity` in separated tread and not block `TC` step code. 

#### BuildSteps Format ####

Comming Soon


### Step 3: Steps Exequting ###

Unity will be ran and call `TCUnityBuild.Build`. It will parse all steps from `-buildSteps` paramert, and if it has mistake, will log `TC Unity Script Fatal Error` with error description.

If all is Ok, `TCUnityBuild.Build` will run steps in order and write logs to selected log file. `TC` should listen and log by `tail` all logs from this file to "tell" `TC` about tests results, write it to `TC Build Logs` and etc. Also `TC` script should handle addition commands from `TC Unity Script`, listed bellow.


#### TC Commands ####

`[TC Unity Script - Fatal Error]` - something went wrong, `TC Unity Script` will close Unity and `TC` should skip all steps;
`[TC Unity Script - Step Completed]` - current step was completed successfully. `TC Unity Script` run next step if it exist, or close `Unity`, if next step not exist. `TC` should close current step as `Successfull` and start next one;
`[TC Unity Script - Step Failed]` - current step was failed, for example build failed, becouse code has compile errors. `TC Unity Script` run next step if it exist, or close `Unity`, if next step not exist. `TC` should close current step as `Failed` and start next one;


### Step 4: Finishing ###

After all steps exequting Unity will be closed automatically. You can write additional steps in `TC`, for example to take Unity licanse back and send `Slack` notifications.


Work inside Unity
---------------------

Comming Soon


Test Steps
---------------------

Comming Soon

