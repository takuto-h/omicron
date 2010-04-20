require 'rake/clean'

EXE = "Omicron.exe"
TEST_FILE = "test.om"
TEST_OUTPUT = "test.output"

CSC = "gmcs"
RUNTIME = "mono"

SRC_DIR = "src"
DEST_DIR = "bin"
SRC_FILES = FileList["#{SRC_DIR}/**/*.cs"]

PROGRAM = "#{DEST_DIR}/#{EXE}"

CLEAN.include(PROGRAM)
CLOBBER.include(DEST_DIR)

task "default" => PROGRAM

file PROGRAM => [DEST_DIR, *SRC_FILES] do
  sh "#{CSC} -out:#{PROGRAM} #{SRC_FILES}"
end

directory DEST_DIR

task "run" => PROGRAM do
  sh "#{RUNTIME} #{PROGRAM}"
end

task "test" => PROGRAM do
  sh "#{RUNTIME} #{PROGRAM} #{TEST_FILE}"
end

task "test-output" => PROGRAM do
  sh "#{RUNTIME} #{PROGRAM} #{TEST_FILE} > #{TEST_OUTPUT}"
end

task "test-diff" => PROGRAM do
  sh "#{RUNTIME} #{PROGRAM} #{TEST_FILE} | diff #{TEST_OUTPUT} -"
end

task "countline" do
  total = SRC_FILES.inject(0) do |sum, name|
    print "#{name}: "
    puts count = IO.readlines(name).size
    sum + count
  end
  puts "Total: #{total}"
end
