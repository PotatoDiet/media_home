import React, { useEffect, useState } from 'react';
import VideoTile from '../components/VideoTile';
import './Videos.css';
import config from '../config';

const Videos = () => {
  const [list, setList] = useState([]);

  useEffect(() => {
    async function grabVideos() {
      const res = await fetch(
        `${config.backendUrl}${window.location.pathname}${window.location.search}`,
      );

      setList(await res.json());
    }
    grabVideos();
  }, [window.location.search]);

  async function update() {
    await fetch(`${config.backendUrl}/videos/update`);
    setList([]);
  }

  async function clean() {
    await fetch(`${config.backendUrl}/videos/clean`);
    setList([]);
  }

  return (
    <div>
      <div className="videos-menu">
        <button type="button" onClick={update}>Update</button>
        <button type="button" onClick={clean}>Clean</button>
      </div>

      <span className="videos">
        {list.map((v) => (
          <VideoTile
            key={v.id}
            id={v.id.toString()}
            title={v.title}
            year={v.year}
            genres={v.genres}
            communityRating={v.communityRating}
            poster={v.poster}
            currentWatchTimestamp={v.currentWatchTimestamp}
          />
        ))}
      </span>
    </div>
  );
};

export default Videos;
