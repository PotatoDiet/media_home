import React from 'react';
import { Switch, Route } from 'react-router-dom';

import { Home } from './pages/Home';
import { Videos } from './pages/Videos';
import Video from './pages/Video';

function Main() {
  return (
    <Switch>
      <Route exact path='/' component={Home}></Route>
      <Route exact path='/videos' component={Videos}></Route>
      <Route path='/video/:id' component={Video}></Route>
    </Switch>
  );
}

export default Main;
