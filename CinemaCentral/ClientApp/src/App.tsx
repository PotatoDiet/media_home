import React from "react";
import {Route, Routes, useNavigate, useRouteError} from 'react-router-dom';
import Home from './pages/Home';
import Movies from './pages/Movies';
import Movie from './pages/Movie';
import './App.css';
import Series from "./pages/Series";
import Episode from "./pages/Episode";
import Login from "./pages/Login";
import ContentLayout from "./components/ContentLayout";

export default function App() {
    return (
        <Routes>
            <Route path="/login" element={<Login />} />
    
            <Route path="/" element={<ContentLayout />}>
                <Route index element={<Home/>}/>
                <Route path="movies" element={<Movies/>}/>
                <Route path="movie/:id" element={<Movie/>}/>
                <Route path="series/:id" element={<Series />}/>
                <Route path="episode/:id" element={<Episode />}/>
            </Route>
        </Routes>
    )
};