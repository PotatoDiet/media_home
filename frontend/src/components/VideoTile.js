import React from 'react';
import { Link } from 'react-router-dom';
import './VideoTile.css';

const VideoTile = props => (
  <Link
    className="video-tile"
    to={`/video/${props.id}`}
    style={{backgroundImage: `url(http://localhost:1234/${props.poster})`}}>
  </Link>
);

export default VideoTile;
