import React from 'react';
import { Link } from 'react-router-dom';
import './MediaTile.css';

type MediaTileProps = {
  poster: string;
  path: string;
}

const MediaTile = ({ poster, path }: MediaTileProps) => (
  <Link
    to={path}
    className="media-tile"
    style={{ backgroundImage: `url(${poster})` }}
  />
);

export default MediaTile;
