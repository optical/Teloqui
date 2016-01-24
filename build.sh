#!/bin/bash

dnvm update-self
dnvm install 1.0.0-rc1-update1
dnvm use 1.0.0-rc1-update1

cd src/Teloqui
dnu restore
dnu build --framework dnx451