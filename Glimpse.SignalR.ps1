#properties ---------------------------------------------------------------------------------------------------------

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\Builds\NuSpec"
    $build_output_dir = "$base_dir\Builds\NuGet"
    $source_dir = "$base_dir\Source"
    $tools_dir = "$base_dir\Tools"
    $config = "release"
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends build

task clean {
    "Cleaning Glimpse.SignalR bin and obj directories"
    delete_directory "$source_dir\Glimpse.SignalR\bin"
    delete_directory "$source_dir\Glimpse.SignalR\obj"

    "Cleaning Glimpse.SignalR.Sample bin and obj directories"
    delete_directory "$source_dir\Glimpse.SignalR.Sample\bin"
    delete_directory "$source_dir\Glimpse.SignalR.Sample\obj"
}

task build -depends clean {
    "Building Glimpse.SignalR.sln"
    exec { msbuild $base_dir\Glimpse.SignalR.sln /p:Configuration=$config }
}

task package -depends build {
    "Creating Glimpse.SignalR.nupkg"
    xcopy $source_dir\Glimpse.SignalR\bin\$config\Glimpse.SignalR.dll $build_dir\Glimpse.SignalR\lib\net40\Glimpse.SignalR.dll /T /E /Y
    xcopy $source_dir\Glimpse.SignalR\Readme.txt $build_dir\Glimpse.SignalR\Content\App_Readme\Glimpse.SignalR.txt /T /E /Y
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack  $build_dir\Glimpse.SignalR\Glimpse.SignalR.nuspec -OutputDirectory $build_output_dir }

    "Creating Glimpse.SignalR.Sample.nupkg"
    xcopy $source_dir\Glimpse.SignalR.Sample\Chat.aspx $build_dir\Glimpse.SignalR.Sample\Content\Chat.aspx /T /E /Y
    xcopy $source_dir\Glimpse.SignalR.Sample\Chat.aspx.cs $build_dir\Glimpse.SignalR.Sample\Content\Chat.aspx.cs /T /E /Y
    xcopy $source_dir\Glimpse.SignalR.Sample\Chat.aspx.designer.cs $build_dir\Glimpse.SignalR.Sample\Content\Chat.aspx.designer.cs /T /E /Y
    xcopy $source_dir\Glimpse.SignalR.Sample\ChatHub.cs $build_dir\Glimpse.SignalR.Sample\Content\ChatHub.cs /T /E /Y
    xcopy $source_dir\Glimpse.SignalR.Sample\Readme.txt $build_dir\Glimpse.SignalR.Sample\Content\App_Readme\Glimpse.SignalR.Sample.txt /T /E /Y
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack  $build_dir\Glimpse.SignalR.Sample\Glimpse.SignalR.Sample.nuspec -OutputDirectory $build_output_dir }
}

#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name) {
    rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}