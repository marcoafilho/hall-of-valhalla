import React from 'react';

import './ConventionSubscriptionFooter.css'

const ConventionSubscriptionFooter = ({ name, onRegister }) => {
    return (
        <footer className="convention-subscription-footer bg-light">
            <div className="container convention-subscription-footer__container">
                <div className="row">
                    <div className="col-9">
                        {name}
                    </div>
                    <div className="col-3">
                        <button type="button" className="btn btn-danger btn-lg float-end" onClick={onRegister}>Register</button>
                    </div>
                </div>
            </div>
        </footer>
    );
};

export default ConventionSubscriptionFooter;