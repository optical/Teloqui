#!/bin/bash

dnvm update-self
dnvm install 1.0.0-rc1-update1
dnvm use 1.0.0-rc1-update1

dnu restore
cd src/Teloqui
dnu build --framework dnx451
dnu build --framework dnxcore50
cd ../Teloqui.PollingSample
dnu build --framework dnx451
dnu build --framework dnxcore50
cd ../Teloqui.Tests
dnu build --framework dnx451
dnu build --framework dnxcore50
dnx test