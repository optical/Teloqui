#!/bin/bash

dnvm update-self
dnvm install 1.0.0-rc1-update1
dnvm use 1.0.0-rc1-update1

dnu restore
dnu build