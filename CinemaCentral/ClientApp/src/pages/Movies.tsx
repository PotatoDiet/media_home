import React, { useEffect, useState } from 'react';
import MovieTile from '../components/MovieTile';
import './Movies.css';

type Movie = {
  id: string;
  posterPath: string;
}

export default function Movies() {
  const [list, setList] = useState([]);

  useEffect( () => {
    async function grabVideos() {
      const res = await fetch(`/api/Movies${window.location.search}`);
      setList(await res.json());
      console.log(list);
    }
    grabVideos();
  }, [window.location.search]);

  async function update() {
    await fetch("/api/Movies/Update", { method: "POST" });
    setList([]);
  }

  async function clean() {
    await fetch("/api/Movies/Clean");
    setList([]);
  }

  return (
    <div>
      <div className="videos-menu">
        <button type="button" onClick={update}>Update</button>
        <button type="button" onClick={clean}>Clean</button>
      </div>

      <span className="videos">
        {list.map((v: Movie) => (
          <MovieTile
            key={v.id}
            id={v.id}
            poster={v.posterPath}
          />
        ))}
      </span>
    </div>
  );
};
