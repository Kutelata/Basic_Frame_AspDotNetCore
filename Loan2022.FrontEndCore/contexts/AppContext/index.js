import React, { createContext, useState } from 'react';

const initialValues = {
    store: {},
    updateStore: () => {
    },
};

export const AppContext = createContext(initialValues);

export const AppProvider = ({ children, initialValues }) => {
    const [state, setState] = useState({ ...initialValues });
    const value = {
        store: state,
        updateStore: (key, payload) => {
            setState((prevState) => {
                return {
                    ...prevState,
                    [key]: payload
                }
            });
        },
    };

    return (
        <AppContext.Provider value={value}>{children}</AppContext.Provider>
    );
}
