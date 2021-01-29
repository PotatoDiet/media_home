import React from 'react';
import VideoTile from '../components/VideoTile';
import './Videos.css'

class Videos extends React.Component {
  constructor() {
    super();
    this.state = {
      list: []
    };

    this.update = this.update.bind(this);
    this.clean = this.clean.bind(this);
  }

  componentDidMount() {
    this.componentDidUpdate();
  }

  async componentDidUpdate() {
    const res = await fetch(
      "http://localhost:1234" + window.location.pathname + window.location.search
    );
    this.setState({ list: await res.json() });
  }

  render() {
    return (
      <div>
        <div class="videos-menu">
          <button onClick={this.update}>Update</button>
          <button onClick={this.clean}>Clean</button>
        </div>

        <span class="videos">
          {this.state.list.map(v => {
            return (
              <VideoTile
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

  async update() {
    await fetch("http://localhost:1234/videos/update");
    this.forceUpdate();
  }

  async clean() {
    await fetch("http://localhost:1234/videos/clean");
    this.forceUpdate();
  }
}

export { Videos };
