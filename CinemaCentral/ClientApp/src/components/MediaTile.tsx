import React from 'react';
import { Link } from 'react-router-dom';
import './MediaTile.css';

type MediaTileProps = {
  poster: string;
  id: string;
  type: string;
}

export default function MediaTile({ poster, id, type }: MediaTileProps) {
  let path = "";
  if (type == "Movie") {
    path = `/movie/${id}`;
  } else if (type === "Series") {
    path = `/series/${id}`;
  }

  return (
    <Link
        to={path}
        className="media-tile"
        style={{backgroundImage: `url(${poster})`}}
    />
  );
}
