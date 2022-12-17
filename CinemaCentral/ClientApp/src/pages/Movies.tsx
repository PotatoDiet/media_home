import React, { useEffect, useState } from 'react';
import MediaTile from '../components/MediaTile';
import './Movies.css';
import {ccFetch} from "../utitilies";
import {Outlet, useNavigate} from "react-router-dom";

type Movie = {
  id: string;
  posterPath: string;
}

export default function Movies() {
  const navigate = useNavigate();
  const [list, setList] = useState([]);

  useEffect( () => {
    async function grabVideos() {
      const res = await ccFetch(`/api/Movies${window.location.search}`,"GET", navigate);
      setList(await res.json());
    }
    grabVideos();
  }, [window.location.search]);

  async function update() {
    await ccFetch("/api/Movies/Update", "POST", navigate);
    setList([]);
  }

  async function clean() {
    await ccFetch("/api/Movies/Clean", "GET", navigate);
    setList([]);
  }

  return (
    <>
      <div className="videos-menu">
        <button type="button" onClick={update}>Update</button>
        <button type="button" onClick={clean}>Clean</button>
      </div>

      <span className="videos">
        {list.map((v: Movie) => (
          <MediaTile
            key={v.id}
            id={v.id}
            type="Movie"
            poster={v.posterPath}
          />
        ))}
      </span>
    </>
  );
};
