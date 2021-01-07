import React from 'react';
import VideoTile from '../components/VideoTile';
import './Videos.css'

class Videos extends React.Component {
  constructor() {
    super();
    this.state = {
      list: []
    };
  }

  async componentDidMount() {
    const res = await fetch("http://localhost:1234/videos");
    this.setState({ list: await res.json() });
  }

  render() {
    return (
      <span class="videos">
        {this.state.list.map(v => {
          return (
            <VideoTile
              id = {v.id}
              title = {v.title}
              year = {v.year}
            />
          )
        })}
      </span>
    );
  }
}

export { Videos };
