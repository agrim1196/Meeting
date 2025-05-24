namespace :docker do
  desc "Build Docker image for the Rails web API"
  task :build do
    image_name = ENV['IMAGE'] || 'meeting-api'
    sh "docker build -t #{image_name} -f meeting-planner/meeting.planner.api/Dockerfile webapi ."
  end
end