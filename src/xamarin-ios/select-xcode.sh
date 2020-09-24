set -e

SYMLINK=$1
if [ -z "$SYMLINK" ]
then
    echo "SYMLINK is not defined"
    exit 0
fi

echo "##vso[task.setvariable variable=MD_APPLE_SDK_ROOT;]/Applications/Xcode_$SYMLINK.app"
sudo xcode-select --switch /Applications/Xcode_$SYMLINK.app/Contents/Developer