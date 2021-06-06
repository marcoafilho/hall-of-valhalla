import React, { Component } from 'react';
import { Collapse, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import SignInButton from './SignInButton';
import SignOutButton from './SignOutButton';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <nav className="navbar bg-light navbar-expand-sm navbar-toggleable-sm ng-white border-bottom shadow mb-3">
                <div className="container-fluid">
                    <NavbarBrand tag={Link} to="/">Hall of Valhalla</NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/profile">Profile</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/talks/new">Register a talk</NavLink>
                            </NavItem>
                            <NavItem>
                                <SignOutButton />
                            </NavItem>
                            <NavItem>
                                <SignInButton />
                            </NavItem>
                        </ul>
                    </Collapse>
                </div>
            </nav>
        );
    }
}
