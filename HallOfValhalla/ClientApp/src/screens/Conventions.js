import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

const Conventions = () => {
    const [conventions, setConventions] = useState([]);

    useEffect(() => {
        const getConventions = async () => {
            const response = await fetch('api/v1/conventions');
            const data = await response.json();

            setConventions(data);
        }

        getConventions();
    }, []);

    return (
        <div className="row">
            <div className="col-8">
                <h1>Upcoming Conventions</h1>
                {conventions.map(convention =>
                    <div key={convention.id} className="card">
                        <div className="card-body">
                            <h5 className="card-title">
                                <Link to={"/conventions/" + convention.id} className="link-dark">
                                    {convention.name}
                                </Link>
                            </h5>
                        </div>
                    </div>
                )}
            </div>
            <div className="col-4" />
        </div>
    );
};

export default Conventions;
