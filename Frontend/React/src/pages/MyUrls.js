import React, { useState, useEffect } from 'react';
import api from '../services/api';
import { getSessionId } from '../utils/session';
import './MyUrls.css';

const MyUrls = ({ user }) => {
    const [myUrls, setMyUrls] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchMyUrls = async () => {
            setLoading(true);
            setError('');
            try {
                const sessionId = getSessionId();
                const headers = {};
                if (!user && sessionId) {
                    headers['X-Session-Id'] = sessionId;
                }

                const response = await api.get('/urls/my-urls', { headers });
                setMyUrls(response.data);
            } catch (err) {
                setError('Failed to fetch your URLs.');
                console.error('Error fetching my URLs:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchMyUrls();
    }, [user]); // Re-fetch if user status changes

    if (loading) {
        return <div className="my-urls-container"><p>Loading your URLs...</p></div>;
    }

    if (error) {
        return <div className="my-urls-container"><p className="error-message">{error}</p></div>;
    }

    return (
        <div className="my-urls-container">
            <h2>My Shortened URLs</h2>
            {myUrls.length === 0 ? (
                <p>You haven't shortened any URLs yet.</p>
            ) : (
                <div className="url-list">
                    {myUrls.map((url) => (
                        <div key={url.id} className="url-item">
                            <p>Original: <a href={url.longUrl} target="_blank" rel="noopener noreferrer">{url.longUrl}</a></p>
                            <p>Short: <a href={url.shortUrl} target="_blank" rel="noopener noreferrer">{url.shortUrl}</a></p>
                            <p className="url-code">Code: {url.code}</p>
                            <p className="url-date">Created: {new Date(url.createdOnUtc).toLocaleString()}</p>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default MyUrls;
