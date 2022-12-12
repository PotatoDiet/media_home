import React from 'react';
import PropTypes from 'prop-types';
import MediaTile from './MediaTile';

export default function MovieTile({ id, poster }) {
  return (
    <MediaTile
        path={`/movie/${id}`}
        poster={poster}
    />
  );
};

MovieTile.propTypes = {
  id: PropTypes.string.isRequired,
  poster: PropTypes.string.isRequired,
};
