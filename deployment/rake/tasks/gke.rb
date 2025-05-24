namespace :gke do
  desc "Build, push Docker image to GCR, and deploy to GKE using Terraform"
  task :deploy do
    project_id = ENV['PROJECT_ID'] || 'golden-system-279417'
    image_tag = ENV['IMAGE_TAG'] || 'latest'
    dockerfile = 'meeting-planner/meeting.planner.api/Dockerfile'
    context = 'meeting-planner/meeting.planner.api'
    image = "gcr.io/#{project_id}/meeting-webapi:#{image_tag}"

    puts "Building Docker image..."
    sh "docker build -t #{image} -f #{dockerfile} #{context}"

    puts "Authenticating Docker to GCR..."
    sh "gcloud auth configure-docker"

    puts "Pushing Docker image to GCR..."
    sh "docker push #{image}"

    puts "Deploying to GKE with Terraform..."
    Dir.chdir('deployment/Terraform') do
      sh "terraform init"
      sh "terraform apply -auto-approve -var='project_id=#{project_id}' -var='image_tag=#{image_tag}'"
    end

    puts "Deployment complete!"
  end
end