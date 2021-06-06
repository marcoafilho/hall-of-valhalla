import React, { useEffect, useState } from 'react';
import { useAuth0 } from "@auth0/auth0-react";

const handleSubmit = (getAccessTokenSilently, user, title, description, conventionId, event) => {
    event.preventDefault();

    const createTalk = async () => {
        const audience = process.env.REACT_APP_AUTH0_AUDIENCE;

        debugger;

        try {
            const accessToken = await getAccessTokenSilently({
                audience: audience,
                scope: "create:talks",
            });

            const updateUserDetailsUrl = `${audience}talks`;

            await fetch(updateUserDetailsUrl, {
                method: 'POST',
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ ConventionId: conventionId, Title: title, Description: description, Speaker: user.name })
            });

        } catch (e) {
            console.log(e.message);
        }
    };

    createTalk();
};

const useInput = initialValue => {
    const [value, setValue] = useState(initialValue);

    return {
        value,
        setValue,
        reset: () => setValue(""),
        bind: {
            value,
            onChange: event => {
                setValue(event.target.value);
            }
        }
    };
};

const NewTalk = () => {
    const { user, getAccessTokenSilently } = useAuth0();
    const { value: title, bind: bindTitle } = useInput('');
    const { value: description, bind: bindDescription } = useInput('');
    const { value: conventionId, bind: bindConventionId } = useInput('');

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
                <h1>Register a Talk</h1>

                <form onSubmit={(event) => handleSubmit(getAccessTokenSilently, user, title, description, conventionId, event)} className="pb-3">
                    <div className="pb-3">
                        <label htmlFor="talk_convention_id" className="form-label">Convention</label>
                        <select id="talk_convention_id" name="ConventionId" className="form-select" {...bindConventionId}>
                            <option>Select a convetion...</option>
                            {conventions.map(convention =>
                                <option key={convention.id} value={convention.id}>{convention.name}</option>
                            )}
                        </select>
                    </div>

                    <div className="pb-3">
                        <label htmlFor="talk_title" className="form-label">Title</label>
                        <input name="title" type="text" name="Title" className="form-control" {...bindTitle} />
                    </div>

                    <div className="pb-3">
                        <label htmlFor="talk_title" className="form-label">Description</label>
                        <textarea name="decription" name="Description" className="form-control" {...bindDescription}></textarea>
                    </div>
                    <div>
                        <button type="submit" className="btn btn-primary">Submit talk</button>
                    </div>
                </form>
            </div>
            <div className="col-4">
            </div>
        </div>
    )
};

export default NewTalk;