import React from 'react';
import { Switch, Route } from 'react-router-dom';

import './Main.css';

import Navbar from './components/Navbar';
import { Home } from './pages/Home';
import { Videos } from './pages/Videos';
import Video from './pages/Video';

function Main() {
  return (
    <div>
      <Navbar />

      <div className="content">
        <Switch>
          <Route exact path='/' component={Home}></Route>
          <Route exact path='/videos' component={Videos}></Route>
          <Route path='/video/:id' component={Video}></Route>
        </Switch>
      </div>
    </div>
  );
}

export default Main;
