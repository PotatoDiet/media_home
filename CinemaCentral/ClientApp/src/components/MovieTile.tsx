import React from 'react';
import MediaTile from './MediaTile';

type MovieTileProps = {
  id: string;
  poster: string;
}

export default function MovieTile({ id, poster }: MovieTileProps) {
  return (
    <MediaTile
        path={`/movie/${id}`}
        poster={poster}
    />
  );
};
