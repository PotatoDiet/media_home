import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import './VideoTile.css';
import config from '../config';

const VideoTile = ({ id, poster }) => (
  <Link
    className="video-tile"
    to={`/video/${id}`}
    style={{ backgroundImage: `url(${config.backendUrl}/${poster})` }}
  />
);

VideoTile.propTypes = {
  id: PropTypes.string.isRequired,
  poster: PropTypes.string.isRequired,
};

export default VideoTile;
