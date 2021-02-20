import React from 'react';
import { Switch, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import Videos from './pages/Videos';
import Video from './pages/Video';
import './App.css';

const App = () => (
  <div>
    <Navbar />

    <div className="content">
      <Switch>
        <Route exact path="/" component={Home} />
        <Route path="/videos" component={Videos} />
        <Route path="/video/:id" component={Video} />
      </Switch>
    </div>
  </div>
);

export default App;
