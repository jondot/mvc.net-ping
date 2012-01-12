require "rubygems"
require "bundler"
Bundler.setup


require 'albacore'
require 'version_bumper'

PROJECT = 'ping'

desc "Build"
msbuild :build => :assemblyinfo do |msb|
  msb.properties :configuration => :Release
  msb.targets :Clean, :Build
  msb.solution = "#{PROJECT}.sln"
end


output :output => [:build] do |out|
  out.from '.'
  out.to 'out'
  out.file "Ping/bin/release/Ping.dll", :as=>"Ping.dll"

  out.file 'README.md'
  out.file 'VERSION'
  out.erb "build/#{PROJECT}.nuspec.erb", :as => "#{PROJECT}.nuspec", :locals => { :version => bumper_version }
end

desc "Create Zip for Github"
zip :zip => :output do | zip |
    zip.directories_to_zip "out"
    zip.output_file = "#{PROJECT}.v#{bumper_version.to_s}.zip"
    zip.output_path = File.dirname(__FILE__)
end

desc "Create nuget pacakge"
task :nu => :output do
  `tools/nuget/nuget.exe p out/#{PROJECT}.nuspec`	
end

assemblyinfo :assemblyinfo do |asm|
  asm.version = bumper_version.to_s
  asm.file_version = bumper_version.to_s
  asm.company_name = "Paracode"
  asm.copyright = "Dotan Nahum, Paracode (c) 2010-2011"
  asm.output_file = "AssemblyInfo.cs"
end

