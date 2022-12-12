/* eslint-disable jsx-a11y/media-has-caption */

import React, {useEffect, useRef} from 'react';
import PropTypes from 'prop-types';

const VideoPlayer = ({id, watchtime}) => {
    const ref = useRef();

    useEffect(() => {
        const listener = setInterval(() => {
            const currWatchtime = Math.floor(ref.current.currentTime);
            fetch(`/api/Movies/${id}/UpdateWatchTimestamp`, {
                method: "POST",
                body: currWatchtime,
                headers: {
                    "Content-Type": "application/json"
                },
            });
        }, 5000);

        return () => {
            clearInterval(listener);
        };
    });

    return (
        <video
            width="1024"
            controls
            onLoadStart={() => {
                ref.current.currentTime = watchtime;
            }}
            ref={ref}
        >
            <source src={`/api/Movies/${id}/Stream`}/>
            Can not play this video
        </video>
    );
};

VideoPlayer.propTypes = {
    id: PropTypes.string.isRequired,
    watchtime: PropTypes.number.isRequired,
};

export default VideoPlayer;
