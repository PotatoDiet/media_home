import React from 'react';
import { Link } from 'react-router-dom';
import './VideoTile.css';

class VideoTile extends React.Component {
  render() {
    return (
      <Link class="video-tile" to={`/video/${this.props.id}`}>
        {this.props.title} ({this.props.year})
      </Link>
    )
  }
}

export default VideoTile;
