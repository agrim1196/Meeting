namespace :docker do
  desc "Build Docker image for the .NET WebAPI"
  task :build_webapi do
    image_name = ENV['IMAGE'] || 'meeting-webapi'
    sh "docker build -t #{image_name} -f meeting-planner/meeting.planner.api/Dockerfile meeting-planner/meeting.planner.api"
  end
end