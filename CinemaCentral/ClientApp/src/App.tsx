import React from "react";
import {Route, Routes} from 'react-router-dom';
import Home from './pages/Home';
import Movies from './pages/Movies';
import Movie from './pages/Movie';
import './App.css';
import Series from "./pages/Series";
import Episode from "./pages/Episode";
import Login from "./pages/Login";
import AdminUsers from "./pages/AdminUsers";
import {NewUser} from "./pages/NewUser";
import AdminLibraries from "./pages/AdminLibraries";
import Layout from "./components/Layout";

export default function App() {
    return (
        <Routes>
            <Route path="/" element={<Layout />}>
                <Route index element={<Home/>}/>
                
                <Route path="/admin/users" element={<AdminUsers />}/>
                <Route path="/admin/users/new" element={<NewUser />}/>
                <Route path="/admin/libraries" element={<AdminLibraries />}/>
        
                <Route path="movies" element={<Movies/>}/>
                <Route path="movie/:id" element={<Movie/>}/>
                <Route path="series/:id" element={<Series />}/>
                <Route path="episode/:id" element={<Episode />}/>
                <Route path="/login" element={<Login />} />
            </Route>
        </Routes>
    )
};