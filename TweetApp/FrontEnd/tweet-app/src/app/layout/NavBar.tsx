import { observer } from "mobx-react-lite";
import { Link, NavLink } from "react-router-dom";
import { Container, Icon, Menu, Image, Dropdown } from "semantic-ui-react";
import { useStore } from "../stores/store";

const NavBar = () => {
	const { userStore } = useStore();
	const { user, logout } = userStore;
	return (
		<Menu inverted fixed="top">
			<Container>
				<Menu.Item as={NavLink} exact to="/" header>
					<Icon name="twitter" style={{ marginRight: "10px" }} size="huge" />
					TweetApp
				</Menu.Item>
				<Menu.Item name="Tweets" as={NavLink} to="/tweets" />
				<Menu.Item name="All Users" as={NavLink} to="/allusers" />
				<Menu.Item name="Post Tweet" as={NavLink} to="/post-tweet" />
				<Menu.Item position="right">
					<Image src="/assets/user.png" avatar spaced="right" />
					{user && (
						<Dropdown pointing="top left" text={user.email}>
							<Dropdown.Menu>
								<Dropdown.Item
									text="My Profile"
									as={Link}
									to="/my-profile"
									icon="user"
								/>
								<Dropdown.Item onClick={logout} text="logout" icon="power" />
							</Dropdown.Menu>
						</Dropdown>
					)}
				</Menu.Item>
			</Container>
		</Menu>
	);
};

export default observer(NavBar);
