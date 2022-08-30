import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Container, Header, Icon, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import LoginForm from "../users/LoginForm";
import RegisterForm from "../users/RegisterForm";

const HomePage = () => {
	const { modalStore, userStore } = useStore();
	return (
		<Segment inverted textAlign="center" vertical className="masthead">
			<Container text>
				<Header as="h1" inverted>
					<Icon name="twitter" size="massive" style={{ marginBottom: 12 }} />
					TweetApp
				</Header>
				<h2>... Post it Now ...</h2>
					<br/>
				{userStore.isLoggedIn ? (
					<>
						<Header as="h2" inverted content="Welcome to TweetApp" />
						<Button as={Link} to="/tweets" size="huge" inverted>
							Go to tweets!
						</Button>
					</>
				) : (
					<>
						<Button
							onClick={() => modalStore.openModal(<LoginForm />)}
							to="/login"
							size="huge"
							inverted
						>
							Login
						</Button>
						<Button
							onClick={() => modalStore.openModal(<RegisterForm />)}
							to="/register"
							size="huge"
							inverted
						>
							Register
						</Button>
					</>
				)}
			</Container>
		</Segment>
	);
};

export default observer(HomePage);
