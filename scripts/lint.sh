#!/bin/bash -xe

cd `dirname $0` && cd ../

FORMAT_SHARED_OPTIONS=" " #"--exclude *"

# Argument
USE_FIXLINT=$1
if [ ! "${USE_FIXLINT}" ]; then
  FORMAT_FIX_OPTIONS=" --verify-no-changes "
fi

dotnet dotnet format whitespace ${FORMAT_SHARED_OPTIONS} ${FORMAT_FIX_OPTIONS}
dotnet dotnet format style ${FORMAT_SHARED_OPTIONS} ${FORMAT_FIX_OPTIONS}