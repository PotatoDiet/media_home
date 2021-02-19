import React, { useEffect, useRef } from "react";

const VideoPlayer = props => {
  const ref = useRef();

  useEffect(() => {
    let listener = setInterval(() => {
      const watchtime = Math.floor(ref.current.currentTime);
      console.log(watchtime);
      fetch(`http://localhost:1234/video/${props.id}/update_watch_timestamp?timestamp=${watchtime}`);
    }, 5000);

    return () => {
      clearInterval(listener);
    }
  });
  
  return (
    <video width="1024"
           controls
           onLoadStart={(ev) => ev.target.currentTime = props.watchtime}
           ref={ref}>
      <source src={`http://localhost:1234/stream/${props.id}`} type="video/mp4" />
      Can't play this video
    </video>
  );
}

export default VideoPlayer;
