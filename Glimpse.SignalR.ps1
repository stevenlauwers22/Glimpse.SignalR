#properties ---------------------------------------------------------------------------------------------------------

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\Builds"
    $source_dir = "$base_dir\Source"
    $tools_dir = "$base_dir\Tools"
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
    exec { msbuild $source_dir\Glimpse.SignalR.sln /p:Configuration=Release }
}

task package -depends build {
    "Creating Glimpse.SignalR.nupkg"
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack $source_dir\Glimpse.SignalR\Package.nuspec -OutputDirectory $build_dir }

    "Creating Glimpse.SignalR.Sample.nupkg"
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack $source_dir\Glimpse.SignalR.Sample\Package.nuspec -OutputDirectory $build_dir }
}

task publish {
    "Publishing Glimpse.SignalR.nupkg"
    $apiKey = Read-Host 'Enter your API key: '
    $version = Read-Host 'Enter the number of the version you want to publish: '
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe push $build_dir\Glimpse.SignalR.$version.nupkg $apiKey }
}

#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name) {
    rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}