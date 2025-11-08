import { v4 as uuidv4 } from 'uuid';

const SESSION_ID_KEY = 'anonymous_session_id';

export const getSessionId = () => {
    let sessionId = localStorage.getItem(SESSION_ID_KEY);
    if (!sessionId) {
        sessionId = uuidv4();
        localStorage.setItem(SESSION_ID_KEY, sessionId);
    }
    return sessionId;
};
