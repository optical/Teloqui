#!/bin/bash

dnvm update-self
dnvm install 1.0.0-rc1-update1
dnvm use 1.0.0-rc1-update1

dnu restore
cd src/Teloqui
dnu build --framework dnx452
cd ../Teloqui.Tests
dnu build --framework dnx452
dnx test