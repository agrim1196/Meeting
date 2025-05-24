FROM ruby:3.2

# Install dependencies
RUN apt-get update -qq && apt-get install -y nodejs postgresql-client

# Set working directory
WORKDIR /myapp

# Copy Gemfiles and install gems
COPY Gemfile* ./
RUN bundle install

# Copy the rest of the app
COPY . .

# Precompile assets (if needed)
# RUN bundle exec rake assets:precompile

# Expose port
EXPOSE 3000

# Start the Rails server
CMD ["rails", "server", "-b", "0.0.0.0"]