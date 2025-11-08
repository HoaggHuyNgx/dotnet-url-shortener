import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import api from './services/api';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Register from './pages/Register';
import MyUrls from './pages/MyUrls'; // Import trang mới
import { getSessionId } from './utils/session'; // Import hàm getSessionId
import './App.css';

// Trang chủ
const HomePage = ({ user }) => {
    const [longUrl, setLongUrl] = useState('');
    const [customCode, setCustomCode] = useState('');
    const [shortenedUrl, setShortenedUrl] = useState(null);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setShortenedUrl(null);

        if (!longUrl) {
            setError('Please enter a URL.');
            return;
        }

        try {
            const payload = { originalUrl: longUrl };
            if (user && customCode) {
                payload.customCode = customCode;
            }

            const response = await api.post('/urls', payload);
            setShortenedUrl(response.data);
        } catch (err) {
            if (err.response && err.response.data) {
                setError(err.response.data);
            } else {
                setError('An error occurred. Please try again.');
            }
        }
    };

    return (
        <div className="App">
            <header className="App-header">
                <h1>URL Shortener</h1>
                <form onSubmit={handleSubmit} className="url-form">
                    <input
                        type="url"
                        value={longUrl}
                        onChange={(e) => setLongUrl(e.target.value)}
                        placeholder="Enter a long URL to shorten"
                        required
                    />
                    {user && (
                        <input
                            type="text"
                            value={customCode}
                            onChange={(e) => setCustomCode(e.target.value)}
                            placeholder="Optional: your-custom-code"
                            className="custom-code-input"
                        />
                    )}
                    <button type="submit">Shorten</button>
                </form>

                {error && <p className="error">{error}</p>}

                {shortenedUrl && (
                    <div className="result">
                        <p>Your shortened URL is ready:</p>
                        <a href={shortenedUrl.shortUrl} target="_blank" rel="noopener noreferrer">
                            {shortenedUrl.shortUrl}
                        </a>
                    </div>
                )}
            </header>
        </div>
    );
};

function App() {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        if (storedUser) {
            setUser(JSON.parse(storedUser));
        } else {
            // Nếu không có user, đảm bảo có session ID được tạo
            getSessionId();
        }
    }, []);

    const handleLoginSuccess = (userData) => {
        localStorage.setItem('user', JSON.stringify(userData));
        setUser(userData);
    };

    const handleLogout = () => {
        localStorage.removeItem('user');
        setUser(null);
        // Khi logout, cũng nên tạo một session ID mới cho phiên ẩn danh tiếp theo
        localStorage.removeItem('anonymous_session_id');
        getSessionId();
    };

    return (
        <Router>
            <Navbar user={user} onLogout={handleLogout} />
            <main>
                <Routes>
                    <Route path="/" element={<HomePage user={user} />} />
                    <Route path="/login" element={<Login onLoginSuccess={handleLoginSuccess} />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/my-urls" element={<MyUrls user={user} />} /> {/* Thêm route mới */}
                </Routes>
            </main>
        </Router>
    );
}

export default App;
