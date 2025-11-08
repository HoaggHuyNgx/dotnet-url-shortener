import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

const Navbar = ({ user, onLogout }) => {
    return (
        <nav className="navbar">
            <Link to="/" className="navbar-brand">URL Shortener</Link>
            <div className="navbar-links">
                <Link to="/my-urls" className="navbar-button">My URLs</Link>
                {user ? (
                    <>
                        <span className="navbar-user">Welcome, {user.username}!</span>
                        <button onClick={onLogout} className="navbar-button">Logout</button>
                    </>
                ) : (
                    <>
                        <Link to="/login" className="navbar-button">Login</Link>
                        <Link to="/register" className="navbar-button register-btn">Register</Link>
                    </>
                )}
            </div>
        </nav>
    );
};

export default Navbar;
