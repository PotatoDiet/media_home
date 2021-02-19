import React, { useEffect, useRef, useState } from "react";
import VideoTile from "../components/VideoTile";
import "./Videos.css"

const Videos = () => {
  const isMounted = useRef(true);

  const [list, setList] = useState([]);

  useEffect(() => {
    async function grabVideos() {
      const res = await fetch(
        "http://localhost:1234" + window.location.pathname + window.location.search
      );

      if (isMounted.current) {
        setList(await res.json());
      }
    }
    grabVideos();

    return () => {
      isMounted.current = false;
    }
  });

  async function update() {
    await fetch("http://localhost:1234/videos/update");
    setList([]);
  }

  async function clean() {
    await fetch("http://localhost:1234/videos/clean");
    setList([]);
  }

  return (
    <div>
      <div className="videos-menu">
        <button onClick={update}>Update</button>
        <button onClick={clean}>Clean</button>
      </div>

      <span className="videos">
        {list.map(v => {
          return (
            <VideoTile
              key = {v.id}
              id = {v.id}
              title = {v.title}
              year = {v.year}
              genres = {v.genres}
              communityRating = {v.communityRating}
              poster = {v.poster}
              currentWatchTimestamp = {v.currentWatchTimestamp}
            />
          )
        })}
      </span>
    </div>
  );
}

export default Videos;
