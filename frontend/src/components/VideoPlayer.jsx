/* eslint-disable jsx-a11y/media-has-caption */

import React, { useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import config from '../config';

const VideoPlayer = ({ id, watchtime }) => {
  const ref = useRef();

  useEffect(() => {
    const listener = setInterval(() => {
      const currWatchtime = Math.floor(ref.current.currentTime);
      fetch(`${config.backendUrl}/video/${id}/update_watch_timestamp?timestamp=${currWatchtime}`);
    }, 5000);

    return () => {
      clearInterval(listener);
    };
  });

  return (
    <video
      width="1024"
      controls
      onLoadStart={() => { ref.current.currentTime = watchtime; }}
      ref={ref}
    >
      <source src={`${config.backendUrl}/stream/${id}`} type="video/mp4" />
      Can not play this video
    </video>
  );
};

VideoPlayer.propTypes = {
  id: PropTypes.string.isRequired,
  watchtime: PropTypes.number.isRequired,
};

export default VideoPlayer;
