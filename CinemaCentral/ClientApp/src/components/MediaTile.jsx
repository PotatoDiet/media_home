import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import './MediaTile.css';

const MediaTile = ({ poster, path }) => (
  <Link
    to={path}
    className="media-tile"
    style={{ backgroundImage: `url(${poster})` }}
  />
);

MediaTile.propTypes = {
  path: PropTypes.string.isRequired,
  poster: PropTypes.string.isRequired,
};

export default MediaTile;
