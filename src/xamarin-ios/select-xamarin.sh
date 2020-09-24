set -e

SYMLINK=$1
NDK_VERSION=$2
NDK_PATH=""

if [ $NDK_VERSION ]
then
    echo "Set Android NDK $NDK_VERSION"
    NDK_PATH="$ANDROID_HOME/ndk/$NDK_VERSION"
fi

if [ -z "$SYMLINK" ]
then
    # Temporary workaround for xamarin's issue with lost command line tools
    # due incorrect path to macOS SDK, when we use symlink to Xcode.
    # Link to xamarin.ios issue: https://github.com/xamarin/xamarin-macios/issues/8005
    lipo 2>/dev/null || true

    echo "SYMLINK is not defined"
    exit 0
fi

$AGENT_HOMEDIRECTORY/scripts/select-xamarin-sdk.sh $SYMLINK

MONOPREFIX=/Library/Frameworks/Mono.framework/Versions/$SYMLINK
echo "##vso[task.setvariable variable=DYLD_FALLBACK_LIBRARY_PATH;]$MONOPREFIX/lib:/lib:/usr/lib:$DYLD_LIBRARY_FALLBACK_PATH"
echo "##vso[task.setvariable variable=PKG_CONFIG_PATH;]$MONOPREFIX/lib/pkgconfig:$MONOPREFIX/share/pkgconfig:$PKG_CONFIG_PATH"
echo "##vso[task.setvariable variable=PATH;]$NDK_PATH:$MONOPREFIX/bin:$PATH"