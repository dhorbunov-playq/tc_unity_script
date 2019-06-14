TC Unity Script
====================================

Glossary
---------------------

* `TC` - [Team City](https://www.jetbrains.com/teamcity/) Continuous Integration;
* `TC Unity Script` - this repository, made for execute Builds, Tests, and etc, inside `Unity` and from `TC` via command line;
* `TC Step Script` - script of step on `TC`. Can make some logic or just listen logs from `TC Unity Script`.
* `Unified Build Code` - Kotlin code from [tg-unified-build](https://github.com/PlayQ/tg-unified-build) repository, made for automatically create `TC Steps Scripts`.
* `Unity Project` - the current game or another project to make build, run tests, or make another manipulations on `TC`.


Principe of work
---------------------

### Step 1: Installation ###

`TC Step Script` enshure `TC Unity Script` is exist in `Unity Project`.

`TC Unity Script` can be already added to `Unity Project` to use it for convenient build running (see [Work Inside Unity](#work-inside-unity)). So `TC Step Script` should check if it's exists and copy it from this repository if not.

Also `TC Step Script` should check is `Newtonsoft.Json.dll` existed in the `Unity Project` becouse it possible `Unity Project` use `Newtonsoft.Json` libruary, but don't use `TC Unity Script`. If so, `TC Step Script` should remove dublicated `Newtonsoft.Json.dll` from `TC Unity Script` in `Unity Project`.


### Step 2: Run Unity ###

`TC Step Script` call `Unity` with command line arguments:
```
"%Unity%" -projectPath "%ProjectPath%" -logfile "%LogFilePath%" -executeMethod TCUnityBuild.Build -buildSteps %BuildSteps% &
```
Where:
* `%Unity%` - path to `Unity`, for example `/Applications/Unity/Unity 5.6.3.app/Contents/MacOS/Unity`;
* `%ProjectPath%` - path to `Unity Project`, for example `~/Documents/Repositories/CharmKing`;
* `%LogFilePath%` - file to write logs, for example `~/Documents/Repositories/CharmKing/build/BuildLog.txt`;
* `-executeMethod TCUnityBuild.Build` - say `Unity` which method it should call. `TCUnityBuild.Build` is entry point for `TC Unity Script` steps runner. You shouldn't change this one;
* `%BuildSteps%` - data for `TC Unity Script` in `json` format. It containce all needed steps, versions, keys and another data. Formating (spaces, line brakes) doesn't matter. See [BuildSteps Format](#buildsteps-format);
* `&` - should be to run `Unity` in separated thread to not block `TC Step Script` code.

Also you can use any another `Unity` command line arguments, if no restriction don't described in the [step description](#test-steps). For example UI Tests can't work with command line agrument `-batchmode` и `-nographic`. We recomend use additional command line arguments only if it really nessasary.

See [Unity Command Line Agruments Documentation](https://docs.unity3d.com/Manual/CommandLineArguments.html). 


#### BuildSteps Format ####

Comming Soon


### Step 3: Steps Exequting ###

Unity will be ran and call `TCUnityBuild.Build`. It will parse all steps from `-buildSteps` parametr, make setups, run steps in order, and write logs to selected log file. 

`Unified Build Code` should generate `TC` steps according with `-buildSteps` parametrs. If build `-buildSteps` has 3 steps: `Run Unit Tests`, `Run UI Tests`, and `Make Android Build`, `TC` should has this 3 stepts too in this order for correct build proccess displaying.

Every step on `TC Step Script` should listen and `echo` all logs from log file (for example with `tail` utility) to "tell" `TC` about tests results, write it to `TC Build Logs` and etc. Also `TC Step Script` should handle addition commands from `TC Unity Script`, listed bellow, to make transactions between `TC` steps.


#### TC Commands ####

* `[TC Unity Script - Fatal Error]` - something went wrong, for example, `-buildSteps` json has mistakes. `TC Unity Script` will close `Unity` and current `TC Step Script` should skip all another Unity-related `TC` steps;
* `[TC Unity Script - Step Completed]` - current step was completed successfully. `TC Unity Script` run next step if it exist, or close `Unity`, if next step not exist. `TC Step Script` should close current step as `Successfull` and start the next one;
* `[TC Unity Script - Step Failed]` - current step was failed, for example build failed, becouse code has compile errors. `TC Unity Script` run next step if it exist, or close `Unity`, if next step not exist. `TC Step Script` should close current step as `Failed` and start next one;


### Step 4: Finishing ###

After all steps exequting `TC Unity Script` will close `Unity`. You can write additional `TC Step Script`, for example to take `Unity` licanse back and send `Slack` notifications.


Test Steps
---------------------

### Run Tests ###

#### Edit Mode Tests ####

Comming Soon...


#### Play Mode Tests ####

Comming Soon...


#### Performance Tests ####

Comming Soon...


#### Smoke Tests ####

Comming Soon...


### Create Build ###

#### Android Build ####

Comming Soon...


#### iOS  Build ####

Comming Soon...


#### Amazone Build ####

Comming Soon...


#### WebGL Build ####

Comming Soon...


#### Test Build ####

Comming Soon...



### Other ###

#### Build Asset Bundles ####

Comming Soon...


#### Build Unity Package ####

Comming Soon...


Work Inside Unity
---------------------

Comming Soon
