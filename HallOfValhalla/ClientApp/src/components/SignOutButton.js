import React from "react";
import { useAuth0 } from "@auth0/auth0-react";

const SignOutButton = () => {
    const { isAuthenticated, logout } = useAuth0();

    return isAuthenticated && (
        <a className="text-dark nav-link" href="/" onClick={() => logout({ returnTo: window.location.origin })}>
            Sign Out
        </a>
    );
};

export default SignOutButton;