import React from 'react';
import { Link } from 'react-router-dom';
import './VideoTile.css';

class VideoTile extends React.Component {
  render() {
    return (
      <Link
        className="video-tile"
        to={`/video/${this.props.id}`}
        style={{backgroundImage: `url(http://localhost:1234/${this.props.poster})`}}>
      </Link>
    )
  }
}

export default VideoTile;
