#!/usr/bin/env ruby
# A ruby script that finds all markdown files in the docs directory
# and converts/compiles them to html using kramdown (https://kramdown.gettalong.org/rdoc/index.html)
# The converted html content is saved in the stardew-access/compiled-docs directory.
# Use the following command to execute this script:
#   ruby compiler_script.rb
# Note that you have to cd into the docs directory before running the above command

require 'kramdown'
require 'fileutils'

compiled_docs_dir = File.join("..", "stardew-access", "compiled-docs")
FileUtils.mkdir_p(compiled_docs_dir) unless File.directory?(compiled_docs_dir)

puts "Searching for files to convert/compile to html...";

# https://lofic.github.io/tips/ruby-recursive_globbing.html
markdown_files = Dir['**/*.md']
markdown_files.each do|file_name|
  # Skips the changelogs directory
  next if File.dirname(file_name) == File.join("changelogs")

  puts "Found: " + file_name + ", compiling..."

  source_file_object = File.open(file_name, "r")
  compiled_file_name = File.join(compiled_docs_dir, file_name.sub(".md", ".html"))
  # Create parent dirs if not exit (ref: https://stackoverflow.com/a/12617369/12026423)
  dirname = File.dirname(compiled_file_name)
  FileUtils.mkdir_p(dirname) unless File.directory?(dirname)
  compiled_file_object = File.open(compiled_file_name, "w")

  file_contents = source_file_object.read()
  compiled_html_content = Kramdown::Document.new(file_contents).to_html
  compiled_html_content = compiled_html_content.gsub(".md", ".html")
  compiled_file_object.syswrite(compiled_html_content)

  puts "File " + file_name + " compiled and saved at: " + compiled_file_name
  source_file_object.close()
  compiled_file_object.close()
end
