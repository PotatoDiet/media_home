# Media Home
Just a simple Jellyfin-like media server built with Go and React. Really isn't
much use other than as a fun project.

# Getting Started
### Backend
    # Copy any mp4 to videos/The Shawshank Redemption (1994).mp4
    $ ./setup_videos
    $ go build -o bin/media_home_backend
    $ bin/media_home_backend

### Frontend
    $ npm start
    # Goto localhost:3000/videos, and click 'Update'

# Licensing
Licensed under AGPL-3.0-only
